const express = require('express');
const { graphqlHTTP } = require('express-graphql');
const mongoose = require('mongoose');
const schemaDB = require('./schema/schema');
const resolvers = require('./resolvers/generation');

const app = express();

app.use(express.json());

app.use(
  '/graphql',
  graphqlHTTP({
  schema: schemaDB,
  rootValue: resolvers,
  graphiql: true,
  })
);

const start = async () => {
  mongoose.connect('mongodb://localhost:27017/Solar-Power-Generation', {
    useNewUrlParser: true,
    useUnifiedTopology: true,
  });

  const PORT = process.env.PORT || 3002;
  app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}/graphql`);
  });
  
};

start();

