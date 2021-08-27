import { houseContext } from '../house.context';
import { House, Review } from '../house.model';
import { HouseRepository } from './house.repository';

export const dbRepository: HouseRepository = {
  getHouseList: async () => {
    return await houseContext.find().lean();
  },
  getHouse: async (id: string) => {
    return await houseContext.findOne({ _id: id }).lean();
  },
  saveHouse: async (house: House) => {
    return await houseContext
      .findOneAndUpdate(
        {
          _id: house._id,
        },
        {
          $set: house,
        },
        {
          upsert: true,
          new: true,
        }
      )
      .lean();
  },
  deleteHouse: async (id: string) => {
    const { deletedCount } = await houseContext.deleteOne({ _id: id });
    return deletedCount === 1;
  },
  insertHouseReview: async (id: string, review: Review) => {
    const house = await houseContext.findOne({ _id: id }).lean();
    if (house && review) {
      house.reviews.push(review);
      dbRepository.saveHouse(house);
      return house;
    }
  },
};
