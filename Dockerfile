FROM microsoft/dotnet:latest

# Create directory for the app source code
RUN mkdir -p /app
WORKDIR /app

# Copy the source and restore dependencies
COPY ./Erebus /app
WORKDIR /app/Server/Erebus.Server
RUN dotnet restore

ENV ASPNETCORE_URLS http://+:5000

# Expose the port and start the app
EXPOSE 5000
CMD [ "dotnet", "run" ]



#FROM microsoft/aspnetcore:1.1
# 
#RUN apt-get update
#RUN apt-get install -y nginx
# 
#WORKDIR /app
#COPY bin/Debug/netcoreapp1.1/publish .
# 
##COPY ./DockerConfig/startup.sh .
##RUN chmod 755 /app/startup.sh
# 
#RUN rm /etc/nginx/nginx.conf
#COPY ./DockerConfig/nginx.conf /etc/nginx
# 
#ENV ASPNETCORE_URLS http://+:5000
#EXPOSE 5000 80
# 
##CMD ["sh", "/app/startup.sh"]
#
#RUN service nginx start
#ENTRYPOINT ["dotnet", "Erebus.Server.dll"]