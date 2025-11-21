require("dotenv").config(); // Load environment variables first
const express = require("express");
const mongoose = require("mongoose");
const cors = require("cors");

const app = express();
const PORT = process.env.PORT || 5000;
const MONGO_URI =
  process.env.MONGO_URI

// Middleware
app.use(express.json());
app.use(cors());

// Connect to MongoDB
mongoose
  .connect(MONGO_URI)
  .then(() => console.log("MongoDB connected"))
  .catch((err) => {
    console.error("❌ MongoDB connection error:", err);
    process.exit(1); // Exit process if DB connection fails
  });

// Routes
try {
  app.use("/api/auth", require("./routes/authRoutes"));
  app.use("/api/progress", require("./routes/progressRoutes"));
  app.use("/api/leaderboard", require("./routes/leaderboardRoutes"));
  app.use("/api/profile", require("./routes/profileRoutes"));
  console.log("Routes loaded successfully");
} catch (err) {
  console.error("❌ Error loading routes:", err);
}

app.listen(PORT, () => console.log(`🚀 Server running on port ${PORT}`));
