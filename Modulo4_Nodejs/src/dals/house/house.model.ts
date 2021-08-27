export interface House {
  _id: string;
  listing_url: string;
  name: string;
  summary: string;
  space: string;
  description: string;
  neighborhood_overview: string;
  notes: string;
  transit: string;
  access: string;
  interaction: string;
  house_rules: string;
  property_type: string;
  room_type: string;
  bed_type: string;
  minimum_nights: string;
  maximum_nights: string;
  cancellation_policy: string;
  last_scraped: Date;
  calendar_last_scraped: Date;
  first_review: Date;
  last_review: Date;
  accommodates: Number;
  bedrooms: Number;
  beds: Number;
  number_of_reviews: Number;
  bathrooms: Number;
  amenities: Array<string>;
  price: Number;
  security_deposit: Number;
  cleaning_fee: Number;
  extra_people: Number;
  guests_included: Number;
  images: Object;
  host: Object;
  address: Object;
  availalability: Object;
  review_scores: Object;
  reviews?: Array<Review>;
}

export interface Review {
  _id: string;
  name: string;
  comment: string;
  date: Date;
}
