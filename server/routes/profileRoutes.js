const express = require("express");
const Progress = require("../models/Progress");
const router = express.Router();

// Helper: Convert "mm:ss" to seconds
function timeStringToSeconds(timeStr) {
  const [min, sec] = timeStr.split(":").map(Number);
  return min * 60 + sec;
}

// Helper: Convert seconds to "mm:ss"
function secondsToTimeString(totalSeconds) {
  const minutes = Math.floor(totalSeconds / 60);
  const seconds = totalSeconds % 60;
  return `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(
    2,
    "0"
  )}`;
}

// Route: GET user profile data
router.get("/:username", async (req, res) => {
  const username = req.params.username;

  try {
    // Get all completed progress records for the user
    const completedLevels = await Progress.find({
      username,
      levelComplete: true,
    });

    const levelsCompleted = completedLevels.length;
    const totalStars = completedLevels.reduce(
      (sum, entry) => sum + (entry.stars || 0),
      0
    );

    const timesInSeconds = completedLevels.map((entry) =>
      timeStringToSeconds(entry.timeTaken)
    );
    const averageSeconds =
      timesInSeconds.length > 0
        ? Math.floor(
            timesInSeconds.reduce((a, b) => a + b, 0) / timesInSeconds.length
          )
        : 0;

    const averageTime = secondsToTimeString(averageSeconds);

    res.json({
      username,
      levelsCompleted,
      totalStars,
      averageTime,
    });
  } catch (err) {
    console.error("❌ Error fetching profile data:", err.message);
    res.status(500).json({ msg: "Server error while fetching profile" });
  }
});

// Route: Get unlocked levels
router.get("/unlocked/:username", async (req, res) => {
  const username = req.params.username;

  try {
    const completedLevels = await Progress.find({
      username,
      levelComplete: true,
    }).select("levelNumber");

    const unlocked = [1]; // Level 1 is always unlocked

    for (let i = 1; i <= 3; i++) {
      const isCompleted = completedLevels.some(
        (entry) => entry.levelNumber === i
      );
      if (isCompleted && i + 1 <= 3) {
        unlocked.push(i + 1);
      }
    }

    res.json({ unlockedLevels: unlocked });
  } catch (err) {
    console.error("Error checking unlocked levels:", err.message);
    res
      .status(500)
      .json({ msg: "Server error while checking unlocked levels" });
  }
});

module.exports = router;
