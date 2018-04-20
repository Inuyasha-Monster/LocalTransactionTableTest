todo:
1、实现serivce方法拦截：Cache应用 --ok
2、代码整理分离 迁移到 YiXinFramework
3、实现 Polly 重试容错处理
4、实现发送消息错误报警接口设计(打算先实现邮件提醒)
5、集成分布式缓存
6、启用消费者负载均衡
7、启用rabbitmq集群模式测试
8、集成服务发现和治理(多个生产者均衡负载，但是数据库目前是同一个的模式)
9、集成quartz-ui管理界面
10、集成hangfire-RAM内存模式测试
11、集成kafka消息队列测试
12、关于解析dbmessage通过eventbus发送，做反射优化以及缓存提升性能
13、独立出来一个 MessageDbContext 发布nuget package方便使用，在使用migration迁移即可