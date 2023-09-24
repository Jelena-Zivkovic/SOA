const { buildSchema } = require('graphql');

module.exports = buildSchema(`
    type Generation {
        _id: ID!
        DATE_TIME: String!
        PLANT_ID: Int!
        SOURCE_KEY: String!
        DC_POWER: Float!
        AC_POWER: Float!
        DAILY_YIELD: Float!
        TOTAL_YIELD: Float!
    }

    input GenerationInput {
        DATE_TIME: String!
        PLANT_ID: Int!
        SOURCE_KEY: String!
        DC_POWER: Float!
        AC_POWER: Float!
        DAILY_YIELD: Float!
        TOTAL_YIELD: Float!
    }

    input UpdateGenerationInput {
        DATE_TIME: String!
        PLANT_ID: Int!
        SOURCE_KEY: String!
        DC_POWER: Float!
        AC_POWER: Float!
        DAILY_YIELD: Float!
        TOTAL_YIELD: Float!
    }

    type Query {
        generationById(id: ID!): Generation
        getAll: [Generation]
        searchGenerationsByDailyYield(daily_yield_from: Float!, daily_yield_to: Float!) : [Generation]
        searchGenerationsByTotalYield(total_yield_from: Float!, total_yield_to: Float!) : [Generation]
    }

    type Mutation {
        createGeneration(input: GenerationInput!): Generation
        updateGeneration(id: ID!, input: UpdateGenerationInput!): Generation
        deleteGeneration(id: ID!): Boolean
    }

    schema {
        query: Query
        mutation: Mutation
    }
`);

