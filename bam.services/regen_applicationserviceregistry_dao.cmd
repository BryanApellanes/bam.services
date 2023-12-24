

bamdb.exe /gsr /ta:.\bam.net.dll /sn:ServiceRegistry /fns:Bam.Net.CoreServices.ServiceRegistration.Data /cfi:yes /uis:no /ws:C:\bam\src\generated\Bam.Net.CoreServices\ServiceRegistration\Data\Generated_Dao

# convert this to
# bamdb.exe code generate --from ./bam.net.dll --to c:/bam/src/generated/_the_rest --schemaName ServiceRegistry --fromNameSpace Bam.Net.CoreServices.ServiceRegistration.Data --checkForIds yes --useInheritanceSchema no 