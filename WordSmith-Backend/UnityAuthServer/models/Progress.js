const mongoose = require("mongoose");

const ProgressSchema = new mongoose.Schema({
  username: { type: String, required: true }, // Changed from userId to username
  levelNumber: { type: Number, required: true },
  stars: { type: Number, required: true },
  timeTaken: { type: String, required: true },
  levelComplete: { type: Boolean, default: false },
});

module.exports = mongoose.model("Progress", ProgressSchema);
