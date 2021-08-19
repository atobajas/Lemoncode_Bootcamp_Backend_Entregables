import * as model from 'dals/house/review.model';
import * as apiModel from './review.api-model';

export const mapReviewFromModelToApi = (
  review: model.Review
): apiModel.Review => ({
  name: review.name,
  comment: review.comment,
<<<<<<< HEAD
  date: review.date.toISOString(),
=======
  date: review.date,
>>>>>>> 86983236bbbcf54e8f489c158b69206a3398046f
});

export const mapReviewListFromModelToApi = (
  reviewList: model.Review[]
): apiModel.Review[] => reviewList.map(mapReviewFromModelToApi);

export const mapReviewFromApiToModel = (
  review: apiModel.Review
): model.Review => ({
  name: review.name,
  comment: review.comment,
<<<<<<< HEAD
  date: new Date(),
=======
  date: review.date,
>>>>>>> 86983236bbbcf54e8f489c158b69206a3398046f
});
