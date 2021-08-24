import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { House } from './house.model';
import { Review } from './review.model';

const reviewSchema = new Schema({
  _id: { type: Schema.Types.String, required: true },
  name: { type: Schema.Types.String },
  comment: { type: Schema.Types.String },
  date: { type: Schema.Types.Date },
} as SchemaDefinition<Review>);

const houseSchema = new Schema({
  _id: { type: Schema.Types.String, required: true },
  listing_url: { type: Schema.Types.String },
  name: { type: Schema.Types.String, required: true },
  summary: { type: Schema.Types.String },
  space: { type: Schema.Types.String },
  description: { type: Schema.Types.String },
  last_scraped: { type: Schema.Types.Date },
  amenities: [{ type: Schema.Types.String }],
  price: { type: Schema.Types.Number },
  address: [{ type: Schema.Types.Mixed }],
  reviews: [{ type: reviewSchema }],
} as SchemaDefinition<House>);

export const houseContext = mongoose.model<House>(
  'listingsandreviews',
  houseSchema
);
