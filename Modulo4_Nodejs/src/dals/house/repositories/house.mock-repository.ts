import { ObjectId } from 'mongodb';
import { HouseRepository } from './house.repository';
import { db } from 'dals/mock-data';
import { House } from '..';

export const mockRepository: HouseRepository = {
  getHouseList: async () => db.houses,
  getHouse: async (id: string) =>
    db.houses.find((h) => h._id.toHexString() === id),
  saveHouse: async (house: House) =>
    Boolean(house._id) ? updateHouse(house) : insertHouse(house),
  deleteHouse: async (id: string) => {
    db.houses = db.houses.filter((h) => h._id.toHexString() !== id);
    return true;
  },
};

const insertHouse = (house: House) => {
  const _id = new ObjectId();
  const newHouse: House = {
    ...house,
    _id,
  };

  db.houses = [...db.houses, newHouse];
  return newHouse;
};

const updateHouse = (house: House) => {
  db.houses = db.houses.map((h) =>
    h._id.toHexString() === house._id.toHexString() ? { ...h, ...house } : h
  );
  return house;
};
