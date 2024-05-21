def Analiz(pathfile,step,sec):
    import pandas as pd
    import numpy as np
    import matplotlib.pyplot as plt
    import os
    import datetime
    import time
    start_time = time.time()
    # partie zip
    #zip_file_path = "chemin/vers/votre/fichier.zip"

    # Dossier de destination pour extraire les fichiers
    #extract_folder = "chemin/vers/dossier/de/destination"

    # Cr�er un objet ZipFile
    #with zipfile.ZipFile(zip_file_path, 'r') as zip_ref:
    # Extraire le contenu du fichier zip dans le dossier de destination
    #zip_ref.extractall(extract_folder) """


    #
    # reflist: list of epc in each box
    reflist=pd.DataFrame()
    # 
    files=os.listdir(pathfile)
    for file in files:
        #print(file)
        if file.startswith('reflist_'):
            temp=pd.read_csv(os.path.join(pathfile,file),sep=',').reset_index(drop=True)[['Epc']]
            temp['refListId']=file.split('.')[0]
            #reflist=reflist.append(temp)
            reflist = pd.concat([reflist, temp],axis=0) 
    reflist=reflist.rename(columns={'refListId':'refListId_actual'})
    reflist['refListId_actual']=reflist['refListId_actual'].apply(lambda x:int(x[8:]))
    Q_refListId_actual=reflist.groupby('refListId_actual')['Epc'].nunique().rename('Q refListId_actual').reset_index(drop=False)
    reflist=pd.merge(reflist,Q_refListId_actual,on='refListId_actual',how='left')
    reflist

    # pathfile=r'data_anonymous'
    # 
    # df : rfid readings
    # Chargement des lectures RFID
    df=pd.DataFrame()
    # 
    files=os.listdir(pathfile)
    for file in files:
        #print(file)
        if file.startswith('ano_APTags'):
            temp=pd.read_csv(os.path.join(pathfile,file),sep=',')
            #df=df.append(temp)
            df = pd.concat([df, temp],axis=0) 
    df['LogTime']=pd.to_datetime (df['LogTime'] ,format='%Y-%m-%d-%H:%M:%S') 
    df['TimeStamp']=df['TimeStamp'].astype(float)
    df['Rssi']=df['Rssi'].astype(float)
    df=df.drop(['Reader','EmitPower','Frequency'],axis=1).reset_index(drop=True)
    df=df[['LogTime', 'Epc', 'Rssi', 'Ant']]
    # antennas 1 and 2 are facing the box when photocell in/out 
    Ant_loc=pd.DataFrame({'Ant':[1,2,3,4],'loc':['in','in','out','out']})
    df=pd.merge(df,Ant_loc,on=['Ant'])
    df=df.sort_values('LogTime').reset_index(drop=True)

    tags=df


    # Chargement des timings
    # timing: photocells a time window for each box: start/stop (ciuchStart, ciuchStop)
    file=r'ano_supply-process.2019-11-07-CUT.csv'
    timing=pd.read_csv(os.path.join(pathfile,file),sep=',')
    timing['file']=file
    timing['date']=pd.to_datetime(timing['date'],format='%d/%m/%Y %H:%M:%S,%f')
    timing['ciuchStart']=pd.to_datetime(timing['ciuchStart'],format='%d/%m/%Y %H:%M:%S,%f')
    timing['ciuchStop']=pd.to_datetime(timing['ciuchStop'],format='%d/%m/%Y %H:%M:%S,%f')
    timing['timestampStart']=timing['timestampStart'].astype(float)
    timing['timestampStop']=timing['timestampStop'].astype(float)
    timing=timing.sort_values('date')
    timing.loc[:,'refListId']=timing.loc[:,'refListId'].apply(lambda x:int(x[8:]))
    timing=timing[['refListId', 'ciuchStart', 'ciuchStop']]

    # Cr�ation des fen�tres temporelles
    # ciuchStart_up starts upstream ciuchStart, half way in between the previous stop and the actual start
    timing[['ciuchStop_last']]=timing[['ciuchStop']].shift(1)
    timing[['refListId_last']]=timing[['refListId']].shift(1)

    timing['ciuchStartup']=timing['ciuchStart'] - (timing['ciuchStart'] - timing['ciuchStop_last'])/2
    # timing start: 10sec before timing
    timing.loc[0,'refListId_last']=timing.loc[0,'refListId']
    timing.loc[0,'ciuchStartup']=timing.loc[0,'ciuchStart']-datetime.timedelta(seconds=sec)
    timing.loc[0,'ciuchStop_last']=timing.loc[0,'ciuchStartup']-datetime.timedelta(seconds=sec)
    timing['refListId_last']=timing['refListId_last'].astype(int)
    # 
    timing['ciuchStopdown']= timing['ciuchStartup'].shift(-1)
    timing.loc[len(timing)-1,'ciuchStopdown']=timing.loc[len(timing)-1,'ciuchStop']+datetime.timedelta(seconds=sec)
    timing=timing[['refListId', 'refListId_last','ciuchStartup', 'ciuchStart','ciuchStop','ciuchStopdown']]


    # box 0 always starts
    timing[timing['refListId']==0].head()

    # t0_run = a new run starts when box 0 shows up
    t0_run=timing[timing['refListId']==0] [['ciuchStartup']]
    t0_run=t0_run.rename(columns={'ciuchStartup':'t0_run'})
    t0_run=t0_run.groupby('t0_run').size().cumsum().rename('run').reset_index(drop=False)
    t0_run=t0_run.sort_values('t0_run')
    # 
    # each row in timing is merged with a last row in t0_run where t0_run (ciuchstart) <= timing (ciuchstart)
    timing=pd.merge_asof(timing,t0_run,left_on='ciuchStartup',right_on='t0_run', direction='backward')
    timing=timing.sort_values('ciuchStop')
    timing=timing[['run', 'refListId', 'refListId_last', 'ciuchStartup','ciuchStart','ciuchStop','ciuchStopdown','t0_run']]


    #  full window (ciuchStartup > ciuchStopdown) is sliced in smaller slices
    # ciuchStartup > ciuchStart: 11 slices named up_0, up_1, ..., up_10
    # ciuchStart > ciuchStop: 11 slices named mid_0, mid_1, ... mid_10
    # ciuchStop > ciuchStopdown: 11 slices names down_0, down_1, ... down_10
    slices=pd.DataFrame()
    for i, row in timing .iterrows():
        ciuchStartup=row['ciuchStartup']
        ciuchStart=row['ciuchStart']
        ciuchStop=row['ciuchStop']
        ciuchStopdown=row['ciuchStopdown']
        steps=step
    #     
        up=pd.DataFrame(index=pd.date_range(start=ciuchStartup, end=ciuchStart,periods=steps,inclusive='left'))\
            .reset_index(drop=False).rename(columns={'index':'slice'})
        up.index=['up_'+str(x) for x in range(steps-1)]
        #slices=slices.append(up)
        slices=pd.concat([slices, up])
    #     
        mid=pd.DataFrame(index=pd.date_range(start=ciuchStart, end=ciuchStop,periods=steps,inclusive='left'))\
            .reset_index(drop=False).rename(columns={'index':'slice'})
        mid.index=['mid_'+str(x) for x in range(steps-1)]
        #slices=slices.append(mid)
        slices=pd.concat([slices, mid])
    #     
        down=pd.DataFrame(index=pd.date_range(start=ciuchStop, end=ciuchStopdown,periods=steps,inclusive='left'))\
            .reset_index(drop=False).rename(columns={'index':'slice'})
        down.index=['down_'+str(x) for x in range(steps-1)]
        slices=pd.concat([slices, down])
    #     slices=slices.append(up)
    slices=slices.reset_index(drop=False).rename(columns={'index':'slice_id'})
    # 
    timing_slices=pd.merge_asof(slices,timing,left_on='slice',right_on='ciuchStartup',direction='backward')
    timing_slices=timing_slices[['run', 'refListId', 'refListId_last','slice_id','slice',  \
                                'ciuchStartup', 'ciuchStart', 'ciuchStop', 'ciuchStopdown','t0_run']]

    # merge between df and timing
    # merge_asof needs sorted df > df_ref
    df=df[ (df['LogTime']>=timing['ciuchStartup'].min()) & (df['LogTime']<=timing['ciuchStopdown'].max())  ]
    df=df.sort_values('LogTime')
    # 
    # each row in df_ref is merged with the last row in timing where timing (ciuchstart_up) < df_ref (logtime)
    # 
    # df_timing=pd.merge_asof(df_ref,timing,left_on=['LogTime'],right_on=['ciuchStartup'],direction='backward')
    # df_timing=df_timing.dropna()
    # df_timing=df_timing.sort_values('LogTime').reset_index(drop=True)
    # df_timing=df_timing[['run', 'Epc','refListId', 'refListId_last', 'ciuchStartup',\
    #                      'LogTime', 'ciuchStop', 'ciuchStopdown','Rssi', 'loc', 'refListId_actual']]
    # 
    # each row in df_ref is merged with the last row in timing_slices where timing (slice) < df_ref (logtime)
    # 
    df_timing_slices=pd.merge_asof(df,timing_slices,left_on=['LogTime'],right_on=['slice'],direction='backward')
    df_timing_slices=df_timing_slices.dropna()
    df_timing_slices=df_timing_slices.sort_values('slice').reset_index(drop=True)
    df_timing_slices=df_timing_slices[['run', 'Epc','refListId', 'refListId_last', 'ciuchStartup','slice_id','slice','LogTime', \
                        'ciuchStart','ciuchStop', 'ciuchStopdown', 'Rssi', 'loc','t0_run']]


    # df_timing_slices=pd.merge(df_timing_slices, reflist, on='Epc',how='left')
    # df_timing_slices = df_timing_slices [ ~((df_timing_slices['refListId']==0) & (df_timing_slices['refListId_actual']==9)) ]
    # # 
    # df_timing_slices = df_timing_slices [ ~((df_timing_slices['refListId']==9) & (df_timing_slices['refListId_actual']==0)) ]
    # # # 
    # # df_timing_slices = df_timing_slices [ ~((df_timing_slices['refListId']==0) | (df_timing_slices['refListId_actual']==0)) ]

    # df_timing_slices=df_timing_slices.drop(['refListId_actual','Q refListId_actual'],axis=1)

    runs_out=df_timing_slices .groupby('run')['refListId'].nunique().rename('Q refListId').reset_index(drop=False)
    runs_out[runs_out['Q refListId']!=10]

    current_last_windows=timing_slices.drop_duplicates(['run','refListId','refListId_last'])
    current_last_windows=current_last_windows[['run','refListId','refListId_last','ciuchStop']].reset_index(drop=True)

    # runs 16 23 32 40 have missing boxes: discarded
    # also run 1 is the start, no previous box: discarded
    # run 18: box 0 run at the end
    # 
    timing=timing[~timing['run'].isin([1,18,16,23,32,40])]
    timing_slices=timing_slices[~timing_slices['run'].isin([1,18,16,23,32,40])]
    df_timing_slices=df_timing_slices[~df_timing_slices['run'].isin([1,18,16,23,32,40])]

    df_timing_slices=df_timing_slices.sort_values(['LogTime','Epc'])
    # 

    # df_timing_slices['dt']=
    df_timing_slices['dt']=(df_timing_slices['LogTime']-df_timing_slices['t0_run']).apply(lambda x:x.total_seconds())

    df_timing_slices['reflist_run_id'] = df_timing_slices['refListId'].astype(str) +"_"+ df_timing_slices['run'].astype(str)



    # 
    # df_timing_threshold
    # 

    rssi_threshold=-110
    df_timing_slices_threshold=df_timing_slices[df_timing_slices['Rssi']>rssi_threshold]

    # readrate
    # readrate
    round(100*df_timing_slices_threshold.reset_index(drop=False).groupby(['run','loc'])['Epc'].nunique().groupby('loc').mean()\
        /reflist['Epc'].nunique(),2)

    ana = df_timing_slices.groupby(['Epc' ,'slice_id', 'loc','reflist_run_id']) ['Rssi'].max()\
        .unstack('loc', fill_value =- 110).reset_index(drop=False)


    order=pd.DataFrame(timing_slices['slice_id'].unique(), columns=['slice_id'])
    order['order']=order. index




    ana=pd.merge(ana, order, on='slice_id', how='left')
    ana = ana [['Epc','reflist_run_id','slice_id', 'in', 'out', 'order']]


    # Last subslice_id with out>in
    ana_out =ana [ ana['out']>ana['in'] ] \
    .sort_values(['Epc', 'reflist_run_id', 'order'], ascending=False) \
    .drop_duplicates(['Epc', 'reflist_run_id'])
    # first subslice_id with in/out
    ana_in =ana [ ana['in']>ana['out'] ] \
    .sort_values(['Epc', 'reflist_run_id', 'order'], ascending=True) \
    .drop_duplicates(['Epc', 'reflist_run_id'])

    ana = pd.merge(ana_in, ana_out, on=['Epc', 'reflist_run_id'], suffixes=['_IN', '_OUT'], how='inner')\
    .sort_values(['Epc', 'reflist_run_id'])
    ana = pd.merge(ana, reflist, on='Epc', how='left')


    ana['pred_ana_bool']= ana['reflist_run_id'].apply(lambda x:x.split('_')[0]).astype('int64')== ana['refListId_actual']

    true=ana[ana['pred_ana_bool']==True]
    #accuracy= (true.shape[0]/ana.shape[0])*100
    accuracy= (true.shape[0]/ana.shape[0])
    error_margin=100-accuracy
    print("Exactitude globale :"+str(accuracy*100)+"%")
    false=ana[ana['pred_ana_bool']==False]
    #a=["Tags bien class�s","Tags mal class�s"]
    #b=[true.shape[0],false.shape[0]]
    #import seaborn as sns
    #sns.barplot(x=a, y=b)
    #for value in range(9):
        #data=ana[(ana["refListId_actual"] == value) & (ana["pred_ana_bool"] == True)]
        #data2=ana[ana["refListId_actual"] == value]
        #print(data.shape[0]/data2.shape[0])'''

    end_time = time.time()

    # Calcul du temps d'exécution
    execution_time = end_time - start_time

    return accuracy, execution_time
