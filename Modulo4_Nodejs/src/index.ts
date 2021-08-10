import express from 'express';
import cors from 'cors';
import path from 'path';

const app = express();
app.use(express.json());
app.use(
  cors({
    methods: 'GET',
    origin: 'http://localhost:8080',
    credentials: true,
  })
);

app.use('/', express.static(path.resolve(__dirname, '../public')));

// Middleware manejo errores siempre el último.
app.use(async (error, req, res, next) => {
  console.error(error);
  res.sendStatus(500);
});

app.listen(3000, () => {
  console.log('Server ready at port 3000');
});
