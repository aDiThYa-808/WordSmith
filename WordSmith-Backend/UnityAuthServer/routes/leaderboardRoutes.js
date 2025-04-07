const express = require("express");
const Progress = require("../models/Progress");
const router = express.Router();

// Helper: Convert "mm:ss" to total seconds
function timeStringToSeconds(timeString) {
  const [minutes, seconds] = timeString.split(":").map(Number);
  return minutes * 60 + seconds;
}

// Route: GET top 3 players for a specific level
router.get("/:levelNumber", async (req, res) => {
  const levelNumber = parseInt(req.params.levelNumber);

  try {
    // Fetch progress entries for the level
    const progresses = await Progress.find({
      levelNumber,
      levelComplete: true,
    });

    // Sort by timeTaken (converted to seconds)
    const sorted = progresses
      .sort(
        (a, b) =>
          timeStringToSeconds(a.timeTaken) - timeStringToSeconds(b.timeTaken)
      )
      .slice(0, 3); // Top 3 only

    // Format response: username and timeTaken
    const leaderboard = sorted.map((entry, index) => ({
      position: index + 1,
      username: entry.username,
      timeTaken: entry.timeTaken,
    }));

    res.json(leaderboard);
  } catch (err) {
    console.error("❌ Error fetching leaderboard:", err.message);
    res.status(500).json({ msg: "Server error while fetching leaderboard" });
  }
});

module.exports = router;
