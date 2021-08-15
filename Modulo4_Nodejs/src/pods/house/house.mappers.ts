import * as model from 'dals';
import * as apiModel from './house.api-model';

export const mapHouseFromModelToApi = (house: model.House): apiModel.House => ({
  _id: house._id,
  name: house.name,
  //releaseDate: book.releaseDate.toISOString(),
});

export const mapHouseListFromModelToApi = (
  houseList: model.House[]
): apiModel.House[] => houseList.map(mapHouseFromModelToApi);

export const mapHouseFromApiToModel = (house: apiModel.House): model.House => ({
  _id: house._id,
  name: house.name,
  //releaseDate: new Date(book.releaseDate),
});
