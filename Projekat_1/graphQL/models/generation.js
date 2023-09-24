const mongoose = require('mongoose');

const generationSchema = new mongoose.Schema({
    DATE_TIME: Date,
    PLANT_ID: Number,
    SOURCE_KEY: String,
    DC_POWER: Number,
    AC_POWER: Number,
    DAILY_YIELD: Number,
    TOTAL_YIELD: Number
});


const Generation = mongoose.model('Generation', generationSchema)
module.exports = {Generation};

// const weatherSensorSchema = new mongoose.Schema({
//     DATE_TIME: Date,
//     PLANT_ID: Number,
//     SOURCE_KEY: String,
//     AMBIENT_TEMPERATURE: Number,
//     MODULE_TEMPERATURE: Number,
//     IRRADIATION: Number
//   });
  
//   module.exports = mongoose.model('weathersensor', weatherSensorSchema);