import mongoose, { Schema, SchemaDefinition } from 'mongoose';
import { House } from './house.model';

const houseSchema = new Schema({
  _id: { type: Schema.Types.String, required: true },
  listing_url: { type: Schema.Types.String },
  name: { type: Schema.Types.String, required: true },
  summary: { type: Schema.Types.String },
  space: { type: Schema.Types.String },
} as SchemaDefinition<House>);

export const houseContext = mongoose.model<House>(
  'listingsandreviews',
  houseSchema
);
