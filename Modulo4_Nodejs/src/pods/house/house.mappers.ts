import * as model from 'dals';
import * as apiModel from './house.api-model';

export const mapBookFromModelToApi = (book: model.House): apiModel.Book => ({
  id: book.id,
  title: book.title,
  releaseDate: book.releaseDate.toISOString(),
  author: book.author,
});

export const mapBookListFromModelToApi = (
  bookList: model.House[]
): apiModel.Book[] => bookList.map(mapBookFromModelToApi);

export const mapBookFromApiToModel = (book: apiModel.Book): model.House => ({
  id: book.id,
  title: book.title,
  releaseDate: new Date(book.releaseDate),
  author: book.author,
});
