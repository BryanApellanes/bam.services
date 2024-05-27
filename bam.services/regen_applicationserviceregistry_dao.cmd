

bamdb.exe /gsr /ta:.\Bam.dll /sn:ServiceRegistry /fns:Bam.CoreServices.ServiceRegistration.Data /cfi:yes /uis:no /ws:C:\bam\src\generated\Bam.CoreServices\ServiceRegistration\Data\Generated_Dao

# convert this to
# bamdb.exe code generate --from ./Bam.dll --to c:/bam/src/generated/_the_rest --schemaName ServiceRegistry --fromNameSpace Bam.CoreServices.ServiceRegistration.Data --checkForIds yes --useInheritanceSchema no 