const mongoose = require("mongoose");

const ProgressSchema = new mongoose.Schema({
  username: { type: String, required: true },
  levelNumber: { type: Number, required: true },
  stars: { type: Number, required: true },
  timeTaken: { type: String, required: true },
  levelComplete: { type: Boolean, default: false },
});

// Optional: Prevent duplicate level entries for same user
ProgressSchema.index({ username: 1, levelNumber: 1 }, { unique: true });

module.exports = mongoose.model("Progress", ProgressSchema);
