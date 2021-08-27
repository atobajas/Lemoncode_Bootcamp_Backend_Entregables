import { disconnect } from 'mongoose';
import { envConstants } from 'core/constants';
import { connectToDBServer } from 'core/servers';
import { houseContext } from 'dals/house/house.context';

const runQueries = async () => {
  const result = await houseContext
    .findOneAndUpdate(
      {
        _id: { $eq: '77777777' },
      },
      {
        $pull: {
          reviews: {
            _id: { $eq: '00000001' },
          },
        },
      },
      {
        new: true,
        projection: { _id: 1, name: 1, reviews: 1 },
      }
    )
    .lean();
};

export const run = async () => {
  await connectToDBServer(envConstants.MONGODB_URI);
  await runQueries();
  await disconnect();
};
