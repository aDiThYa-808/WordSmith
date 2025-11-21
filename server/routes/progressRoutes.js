const express = require("express");
const Progress = require("../models/Progress");
const router = express.Router();

// Save or update progress for specific level
router.post("/save", async (req, res) => {
  console.log("Received progress data:", req.body);

  const { username, levelNumber, stars, timeTaken, levelComplete } = req.body;

  if (!username || levelNumber === undefined) {
    return res
      .status(400)
      .json({ msg: "Username and levelNumber are required." });
  }

  try {
    let progress = await Progress.findOne({ username, levelNumber });

    if (progress) {
      // Check for improvement before updating
      const existingTime = parseTime(progress.timeTaken);
      const newTime = parseTime(timeTaken);

      const improvedStars = stars > progress.stars;
      const fasterTime = newTime < existingTime;

      if (improvedStars || fasterTime) {
        progress.stars = Math.max(stars, progress.stars);
        progress.timeTaken = fasterTime ? timeTaken : progress.timeTaken;
        progress.levelComplete = levelComplete || progress.levelComplete;

        await progress.save();
        return res.json({ msg: "🔁 Progress updated (better score/time)!" });
      } else {
        return res.json({ msg: "ℹ️ No improvement. Existing progress kept." });
      }
    } else {
      // No existing progress for this level — create new
      const newProgress = new Progress({
        username,
        levelNumber,
        stars,
        timeTaken,
        levelComplete,
      });

      await newProgress.save();
      return res.json({ msg: "New progress saved!" });
    }
  } catch (err) {
    console.error("❌ Server error:", err.message);
    res.status(500).json({ msg: "Server error: " + err.message });
  }
});

// Helper: Convert mm:ss to seconds
function parseTime(timeStr) {
  const [minutes, seconds] = timeStr.split(":").map(Number);
  return minutes * 60 + seconds;
}

module.exports = router;
