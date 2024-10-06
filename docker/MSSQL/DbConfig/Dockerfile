# We choose exact tag (not 'latest'), to be sure that new version won't break creating image
FROM "mcr.microsoft.com/mssql/server:2017-latest-ubuntu"

#FROM "mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04" 

#USER 0

# Create app directory
RUN mkdir -p /usr/src/app

#USER $CONTAINER_USER_ID

WORKDIR /usr/src/app

# Copy initialization scripts
COPY . /usr/src/app

# Grant permissions for the run-initialization script to be executable
RUN chmod +x /usr/src/app/run-initialization.sh

## init varible
# # Get environment variables from docker-compose
# ARG varible exist when dockerfile run
# ARG varFirst
# ARG varSecond
# RUN echo "varFirst value >>>>>>>>> ${varFirst}"
# RUN echo "varSecond value >>>>>>>>>> ${varSecond}"

# Expose port 1433 in case accesing from other container
EXPOSE 1433

# Run Microsoft SQl Server and initialization script (at the same time)
# Note: If you want to start MsSQL only (without initialization script) you can comment bellow line out, CMD entry from base image will be taken
CMD /bin/bash ./entrypoint.sh
