Docker build image:
```
 docker run -d `
	--name bing_food_v4 `
	-p 3000:3000 `
	-p 3001:3001 `
	-p 3002:3002 `
	-v "C:\Users\bing\Projects\bb-food-cart:/app" `
	-v bb-foodcart-postgres:/var/lib/postgresql `
	bing_dotnet_stack_v4
```

Docker create container:
```
 docker run -d `
	--name bing_food_v4 `
	-p 3000:3000 `
	-p 3001:3001 `
	-p 3002:3002 `
	-v "C:\Users\bing\Projects\bb-food-cart:/app" `
	-v bb-foodcart-postgres:/var/lib/postgresql `
	bing_dotnet_stack_v4
```

Get in container:
```
docker exec -it bing_food_v4 bash
```
Duration: 111.6s