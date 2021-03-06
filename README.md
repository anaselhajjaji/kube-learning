My idea is to build step by step a complete complex solution.

# Before start
- Setup kubernetes with minikube: https://gist.github.com/mrbobbytables/d9e5c7224dbba989cf0b8a30d7a231a4
- To see kubernetes dashboard: `minikube dashboard`

# Step 1

Having an ASP.NET Core service with 3 replicas deployed in kubernetes and load balanced using kubernetes load balancer. 

- Deploy the service: `kubectl apply -f step1/aspnetcoreservice.yaml`
- To get service ip address: `kubectl get service aspnetcoreservice`, external ip address will not be attributed because we are in local kubernetes.
- To get access to the service: `minikube service aspnetcoreservice`, minikube will create a tunnel to get access to the service from the host machine.

# Step 2

Add an ASP.NET Core GRPC Service and make the first service talk to it using ASP.NET Core hosted services.

- Deploy the service: `kubectl apply -f step2/aspnetcoregrpcservice.yaml`

# Step 3

- Deploy Kafka to kubernetes using instructions in: https://github.com/Yolean/kubernetes-kafka
