const {Generation}  = require('../models/generation');

const resolvers = {
      generationById: async ({ id }) => {
        return Generation.findById(id).then(result => {
          return { ...result._doc, _id: result._doc._id.toString() };
        })
        .catch(err => {
          throw err;
        });
      },
      getAll: async ({ }) => {
          return Generation.find({}).then(datas => {
            return datas.map(event => {
              return { ...event._doc, _id: event.id };
            });
          })
          .catch(err => {
            throw err;
          });
      },
      searchGenerationsByDailyYield: ({ daily_yield_from, daily_yield_to }) => {
        return Generation.find({}).then(datas => {
          return datas.filter(
            (data) =>
              (!daily_yield_from || data.DAILY_YIELD >= daily_yield_from) &&
              (!daily_yield_to || data.DAILY_YIELD <= daily_yield_to) 
          )
        })
        .catch(err => {
          throw err;
        });
      },
      searchGenerationsByTotalYield: ({ total_yield_from, total_yield_to }) => {
        return Generation.find({}).then(datas => {
          return datas.filter(
            (data) =>
              (!total_yield_from || data.TOTAL_YIELD >= total_yield_from) &&
              (!total_yield_to || data.TOTAL_YIELD <= total_yield_to) 
          )
        })
        .catch(err => {
          throw err;
        });
      },
      createGeneration: async ({ input }) => {
        try {
          const newGeneration = new Generation(input);
          return await newGeneration.save();
        } catch (error) {
          throw new Error('Error creating a new generation');
        }
      },
      updateGeneration: async ({ id, input }) => {
        try {
          return await Generation.findByIdAndUpdate(id, input, { new: true });
        } catch (error) {
          throw new Error('Error updating the generation');
        }
      },
      deleteGeneration: async ({ id }) => {
        try {
          const deletedGeneration = await Generation.findByIdAndRemove(id);
          return deletedGeneration ? true : false;
        } catch (error) {
          throw new Error('Error deleting the generation');
        }
      },
  };

module.exports = resolvers;