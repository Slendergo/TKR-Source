set /p port=Enter port:
docker run --network redis --name WorldServer-%Port% -p %port%:2000 -d -v tkr_resources:/data tkr-worldserver /data/%Port%.conf