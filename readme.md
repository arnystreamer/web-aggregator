# mmt

## Building

`cd backend\Jimx.WebAggregator.API`
`docker build -t jimx.webaggregator.api:1.0 -f Dockerfile ..`

`cd frontend\web-agg-app`
`npm run build`

## Transferring

Save:
`docker save -o wa-api.1_0.tar jimx.webaggregator.api:1.0`
`tar -czf wa-ui.1_0.tar -C .\frontend\web-agg-app\dist\web-agg-app\browser .` 

Transfer:
`scp wa-api.1_0.tar wa-ui.1_0.tar root@server:/root`

Cleanup:
`docker stop Jimx.WebAggregator.API`
`docker rm Jimx.WebAggregator.API`
`docker rmi jimx.webaggregator.api:1.0`

`cd /var/www/wa-ui`
`rm -ri *`

Load:
`docker load -i wa-api.1.0.tar`
`tar -xf wa-ui.1.0.tar -C /var/www/wa-ui`

## Running

`docker run --name Jimx.WebAggregator.API -p 15501:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "GENERAL_BASEURL=(PLACEHOLDER1)" -e "GENERAL_FRONTENDURL=(PLACEHOLDER2)" -e "ASPNETCORE_URLS=http://+:80" -e "ConnectionStrings__DefaultConnection=(PLACEHOLDER3)"  --network=mylocalnet -dt jimx.webaggregator.api:1.0`

`sudo service nginx restart` or `sudo service nginx start`

## Testing

`curl http://localhost:15501/health`