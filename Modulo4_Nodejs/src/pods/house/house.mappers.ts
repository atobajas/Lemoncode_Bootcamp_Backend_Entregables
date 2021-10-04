import * as model from 'dals/house/house.model';
import * as apiModel from './house.api-model';

export const mapHouseFromModelToApi = (house: model.House): apiModel.House => ({
  _id: house._id,
  name: house.name,
  listing_url: house.listing_url,
  description: house.description,
  beds: house.beds,
  bathrooms: house.bathrooms,
  address: house.address,
  last_scraped: house.last_scraped,
  reviews: house.reviews,
});

export const mapHouseListFromModelToApi = (
  houseList: model.House[]
): apiModel.House[] => houseList.map(mapHouseFromModelToApi);

export const mapHouseFromApiToModel = (house: apiModel.House): model.House => ({
  _id: house._id,
  listing_url: house.listing_url,
  name: house.name,
  summary: '',
  space: '',
  description: house.description,
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
  last_scraped: house.last_scraped,
  calendar_last_scraped: new Date(),
  first_review: new Date(),
  last_review: new Date(),
  accommodates: 0,
  bedrooms: 0,
  beds: house.beds,
  number_of_reviews: 0,
  bathrooms: house.bathrooms,
  amenities: [],
  price: 0,
  security_deposit: 0,
  cleaning_fee: 0,
  extra_people: 0,
  guests_included: 0,
  images: [],
  host: [],
  address: house.address,
  availalability: [],
  review_scores: [],
  reviews: house.reviews,
});

export const mapHouseListFromApiToModel = (
  houseList: apiModel.House[]
): model.House[] =>
  Array.isArray(houseList) ? houseList.map(mapHouseFromApiToModel) : [];
