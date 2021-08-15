import { House } from 'dals';

export const paginateHouseList = (
  houseList: House[],
  page: number,
  pageSize: number
): House[] => {
  let paginatedHouseList = [...houseList];
  if (page && pageSize) {
    const startIndex = (page - 1) * pageSize;
    const endIndex = Math.min(startIndex + pageSize, paginatedHouseList.length);
    paginatedHouseList = paginatedHouseList.slice(startIndex, endIndex);
  }

  return paginatedHouseList;
};
