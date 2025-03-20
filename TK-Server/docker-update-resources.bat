del /F /S /Q \\wsl$\docker-desktop-data\version-pack-data\community\docker\volumes\tkr_resources\_data
xcopy /S /Q /Y /F /exclude:resourceignore.txt .\TKR.Shared\resources \\wsl$\docker-desktop-data\version-pack-data\community\docker\volumes\tkr_resources\_data\resources\*

copy .\TKR.Docker\RedisConfig\redis_docker.conf \\wsl$\docker-desktop-data\version-pack-data\community\docker\volumes\redis_volume\_data\redis.conf
xcopy /S /Q /Y /F .\TKR.Docker\ServerConfig \\wsl$\docker-desktop-data\version-pack-data\community\docker\volumes\tkr_resources\_data\*