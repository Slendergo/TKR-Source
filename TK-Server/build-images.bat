docker rmi tkr-worldserver
docker build -t tkr-worldserver -f tkr-worldserver-dockerfile .
docker rmi tkr-app
docker build -t tkr-app -f tkr-app-dockerfile .
pause