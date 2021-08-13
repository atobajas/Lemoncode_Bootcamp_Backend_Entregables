import { Router } from 'express';
import { houseRepository } from 'dals';

export const housesApi = Router();

housesApi
  .get('/', async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      let bookList = await houseRepository.getHouseList();

      if (page && pageSize) {
        const startIndex = (page - 1) * pageSize;
        const endIndex = Math.min(startIndex + pageSize, bookList.length);
        bookList = bookList.slice(startIndex, endIndex);
      }
      res.send(bookList);
    } catch (error) {
      next(error);
    }
  })
  .get('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      //const houseId = Number(id);
      const book = await houseRepository.getHouse(id);
      res.send(book);
    } catch (error) {
      next(error);
    }
  })
  .post('/', async (req, res, next) => {
    try {
      const house = req.body;
      const newHouse = await houseRepository.saveHouse(house);
      res.status(201).send(newHouse);
    } catch (error) {
      next(error);
    }
  })
  .put('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      const houseId = Number(id);
      const house = req.body;
      await houseRepository.saveHouse(house);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  })
  .delete('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      //const houseId = Number(id);
      await houseRepository.deleteHouse(id);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  });
