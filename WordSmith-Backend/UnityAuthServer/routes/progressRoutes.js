const express = require("express");
const Progress = require("../models/Progress");
const router = express.Router();

// Save or update progress
router.post("/save", async (req, res) => {
  console.log("Received progress data:", req.body);

  const { username, stars, timeTaken, levelComplete } = req.body; // Fixed naming

  try {
    let progress = await Progress.findOne({ username });

    if (progress) {
      progress.stars = stars;
      progress.timeTaken = timeTaken; // Ensure field exists in MongoDB
      progress.levelComplete = levelComplete;
    } else {
      progress = new Progress({
        username,
        stars,
        timeTaken,
        levelComplete,
      });
    }

    await progress.save();
    res.json({ msg: "✅ Progress saved successfully" });
  } catch (err) {
    console.error("❌ Server error:", err.message);
    res.status(500).json({ msg: "❌ Server error: " + err.message });
  }
});

module.exports = router;
