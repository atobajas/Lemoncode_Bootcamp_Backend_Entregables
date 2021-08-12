import { HouseRepository } from './house.repository';
import { db } from '../../mock-data';
import { House } from '../house.model';

export const mockRepository: HouseRepository = {
  getHouseList: async () => db.houses,
  getHouse: async (id: string) => db.houses.find((h) => h._id === id),
  saveHouse: async (house: House) =>
    Boolean(house._id) ? updateHouse(house) : insertHouse(house),
  deleteHouse: async (id: string) => {
    db.houses.filter((h) => h._id !== id);
    return true;
  },
};

const insertHouse = (house: House) => {
  const _id = (db.houses.length + 1).toString();
  const newHouse: House = {
    ...house,
    _id,
  };

  db.houses = [...db.houses, newHouse];
  return newHouse;
};

const updateHouse = (house: House) => {
  db.houses = db.houses.map((h) =>
    h._id === house._id ? { ...h, ...house } : h
  );
  return house;
};
