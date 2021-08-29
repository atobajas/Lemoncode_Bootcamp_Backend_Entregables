import { Router } from 'express';
//import jwt from 'jsonwebtoken';
import { authenticationMiddleware } from 'pods/security';
import { houseRepository } from 'dals';
import {
  mapHouseFromApiToModel,
  mapHouseFromModelToApi,
  mapHouseListFromModelToApi,
} from './house.mappers';
import {
  mapReviewFromApiToModel,
  mapReviewFromModelToApi,
  mapReviewListFromModelToApi,
} from 'pods/review/review.mappers';
import { paginateHouseList } from './house.helpers';

export const housesApi = Router();

housesApi
  .get('/', async (req, res, next) => {
    try {
      const page = Number(req.query.page);
      const pageSize = Number(req.query.pageSize);
      const modelHouseList = await houseRepository.getHouseList();
      const paginatedHouseList = paginateHouseList(
        modelHouseList,
        page,
        pageSize
      );

      res.send(mapHouseListFromModelToApi(paginatedHouseList));
    } catch (error) {
      next(error);
    }
  })
  .get('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      const modelHouse = await houseRepository.getHouse(id);
      if (modelHouse) {
        res.send(mapHouseFromModelToApi(modelHouse));
      } else {
        res.sendStatus(404);
      }
    } catch (error) {
      next(error);
    }
  })
  .post('/', authenticationMiddleware, async (req, res, next) => {
    try {
      //const [, token] = req.headers.authorization?.split(' ') || [];
      //const secret = 'my-secret';
      //jwt.verify(token, secret, async (error, userSession) => {
      //if (userSession) {
      const house = req.body;
      const newHouse = await houseRepository.saveHouse(
        mapHouseFromApiToModel(house)
      );
      res.status(201).send(mapHouseFromModelToApi(newHouse));
      //} else {
      //  res.sendStatus(401);
      //}
      //};
    } catch (error) {
      next(error);
    }
  )
  .post('/review/:idHouse', async (req, res, next) => {
    try {
      const { idHouse } = req.params;
      const review = mapReviewFromApiToModel(req.body);
      if (review) {
        const house = await houseRepository.insertHouseReview(idHouse, review);
        if (house) {
          res.status(201).send(mapHouseFromModelToApi(house));
        } else {
          res.sendStatus(404);
        }
      } else {
        res.sendStatus(404);
      }
    } catch (error) {
      next(error);
    }
  })
  .put('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      const modelHouse = mapHouseFromApiToModel({ ...req.body, _id: id });
      await houseRepository.saveHouse(modelHouse);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  })
  .delete('/:id', async (req, res, next) => {
    try {
      const { id } = req.params;
      await houseRepository.deleteHouse(id);
      res.sendStatus(204);
    } catch (error) {
      next(error);
    }
  });
