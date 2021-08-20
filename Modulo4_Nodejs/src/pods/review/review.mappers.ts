import * as model from 'dals/house/review.model';
import * as apiModel from './review.api-model';

export const mapReviewFromModelToApi = (
  review: model.Review
): apiModel.Review => ({
  _id: review._id,
  name: review.name,
  comment: review.comment,
  date: review.date.toISOString(),
});

export const mapReviewListFromModelToApi = (
  reviewList: model.Review[]
): apiModel.Review[] => reviewList.map(mapReviewFromModelToApi);

export const mapReviewFromApiToModel = (
  review: apiModel.Review
): model.Review => ({
  _id: review._id,
  name: review.name,
  comment: review.comment,
  date: new Date(),
});
