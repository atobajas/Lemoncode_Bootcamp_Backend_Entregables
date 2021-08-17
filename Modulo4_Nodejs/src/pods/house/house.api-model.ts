export interface House {
  _id: string;
  name: string;
  listing_url: string;
  description: string;
  beds: Number;
  bathrooms: Number;
  address: Object;
  last_scraped: Date;
}
