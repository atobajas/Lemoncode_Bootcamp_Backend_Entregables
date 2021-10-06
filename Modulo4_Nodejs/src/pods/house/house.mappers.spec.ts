import * as apiModel from './house.api-model';
import * as model from 'dals';
import { mapHouseListFromApiToModel } from './house.mappers';

describe('house.mappers specs', () => {
  describe('mapHouseListFromApiToModel', () => {
    it(`should return empty array when it feeds houseList equals undefined`, () => {
      // Arrange
      const houseList: apiModel.House[] = undefined;

      // Act
      const result: model.House[] = mapHouseListFromApiToModel(houseList);

      // Assert
      expect(result).toEqual([]);
    });

    it(`should return empty array when it feeds houseList equals null`, () => {
      // Arrange
      const houseList: apiModel.House[] = null;

      // Act
      const result: model.House[] = mapHouseListFromApiToModel(houseList);

      // Assert
      expect(result).toEqual([]);
    });

    it(`should return empty array when it feeds houseList equals empty array`, () => {
      // Arrange
      const houseList: apiModel.House[] = [];

      // Act
      const result: model.House[] = mapHouseListFromApiToModel(houseList);

      // Assert
      expect(result).toEqual([]);
    });

    it(`should return one mapped item in array when it feeds houseList equals with one item`, () => {
      // Arrange
      const houseList: apiModel.House[] = [
        {
          _id: '10009990',
          name: 'test-name',
          listing_url: 'house.listing_url',
          description: 'test-description',
          beds: 4,
          bathrooms: 2,
          address: 'test-address',
          last_scraped: new Date('2021-10-05T12:30:00'),
          reviews: [
            {
              name: 'test-review-1',
              comment: 'Commentario 1',
              date: new Date(),
            },
            {
              name: 'test-review-2',
              comment: 'Commentario 2',
              date: new Date(),
            },
          ],
        },
      ];

      // Act
      const result: model.House[] = mapHouseListFromApiToModel(houseList);

      // Assert
      const expectedResult: model.House[] = [
        {
          _id: '10009990',
          name: 'test-name',
          listing_url: 'house.listing_url',
          summary: '',
          space: '',
          description: 'test-description',
          neighborhood_overview: '',
          notes: '',
          transit: '',
          access: '',
          interaction: '',
          house_rules: '',
          property_type: '',
          room_type: '',
          bed_type: '',
          minimum_nights: '',
          maximum_nights: '',
          cancellation_policy: '',
          last_scraped: new Date('2021-10-05T12:30:00'),
          calendar_last_scraped: new Date(),
          first_review: new Date(),
          last_review: new Date(),
          accommodates: 0,
          bedrooms: 0,
          beds: 4,
          number_of_reviews: 0,
          bathrooms: 2,
          amenities: [],
          price: 0,
          security_deposit: 0,
          cleaning_fee: 0,
          extra_people: 0,
          guests_included: 0,
          images: [],
          host: [],
          address: 'test-address',
          availalability: [],
          review_scores: [],
          reviews: [
            {
              name: 'test-review-1',
              comment: 'Commentario 1',
              date: new Date(),
            },
            {
              name: 'test-review-2',
              comment: 'Commentario 2',
              date: new Date(),
            },
          ],
        },
      ];
      expect(result).toEqual(expectedResult);
    });
  });
});
