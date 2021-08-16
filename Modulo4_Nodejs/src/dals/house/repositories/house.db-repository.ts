import { getDBInstance } from 'core/servers';
import { House } from '../house.model';
import { HouseRepository } from './house.repository';

export const dbRepository: HouseRepository = {
  getHouseList: async () => {
    const db = getDBInstance();
    return await db.collection<House>('listingsAndReviews').find().toArray();
  },
  getHouse: async (id: string) => {
    const db = getDBInstance();
    return await db
      .collection<House>('listingsAndReviews')
      .findOne({ _id: id });
  },
  saveHouse: async (house: House) => {
    const db = getDBInstance();
    const { value } = await db
      .collection<House>('listingsAndReviews')
      .findOneAndUpdate(
        {
          _id: house._id,
        },
        {
          $set: house,
        },
        {
          upsert: true,
          returnDocument: 'after',
        }
      );
    return value;
  },
  deleteHouse: async (id: string) => {
    //throw new Error('Not implemented');
    const db = getDBInstance();
    const { deletedCount } = await db
      .collection<House>('listingsAndReviews')
      .deleteOne({ _id: id });
    return deletedCount === 1;
  },
};
