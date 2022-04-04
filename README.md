1. Install redis locally: https://stackexchange.github.io/StackExchange.Redis/Server
2. Modify DbFactory to connect to local redis
3. Complete each use case to pass all tests


**I. Counter**

Parallel processes increment shared global counter. Implement without using distributed lock.
https://redis.io/commands/incrby

II. Bitmap

use case for manipulating bits. 
https://redis.io/commands/setbit

III. Leaderboard

Leaderboard is a list of users with their scores. Supported operations: add member, get top N users, get specific user's rank.
https://redis.io/commands/zadd

