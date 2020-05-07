FROM wowzamedia/wowza-streaming-engine-linux:latest
EXPOSE 1935/tcp
EXPOSE 8086/tcp
EXPOSE 8087/tcp
EXPOSE 8088/tcp

VOLUME ["/usr/local/WowzaStreamingEngine/logs"]

WORKDIR /sbin
ENTRYPOINT ["entrypoint.sh"]