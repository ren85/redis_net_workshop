1. Install redis locally: https://stackexchange.github.io/StackExchange.Redis/Server
2. Modify DbFactory to connect to local redis
3. Complete each use case to pass all tests


**I. Counter**

Parallel processes increment shared global counter. Implement without using distributed lock.  
https://redis.io/commands/incrby

**II. Bitmap**

Use case for manipulating bits.  
https://redis.io/commands/setbit

**III. Leaderboard**

Leaderboard is a list of users with their scores. Supported operations: add member, get top N users, get specific user's rank.  
https://redis.io/commands/zadd

**IV. Queue**

Queue is a list of items that different processes can add items to and retrieve them atomically (i.e. no two processes are guaranteed to get the same item).
https://redis.io/commands/lpush

**V. Bloom filter**

Bloom filter is a probabilistic data structure that allows to check for uniqueness efficiently
https://redis.io/commands/setbit

**VI. Distributed lock**

Locking for inter-process coordination
https://redis.io/commands/set/

**VII. Autocomplete**

Very efficient autocomplete implementation
https://redis.io/commands/zadd





