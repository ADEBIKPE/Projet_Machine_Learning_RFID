



def viz(ana):

    true=ana[ana['pred_ana_bool']==True]
    true.shape
    accuracy= true.shape[0]/ana.shape[0]
    print("Exactitude globale :"+str(accuracy*100)+"%")
    false=ana[ana['pred_ana_bool']==False]
    false.shape
    a=["Tags bien classés","Tags mal classés"]
    b=[true.shape[0],false.shape[0]]
    import seaborn as sns
    sns.barplot(x=a, y=b)
    for value in range(9):
        data=ana[(ana["refListId_actual"] == value) & (ana["pred_ana_bool"] == True)]
        data2=ana[ana["refListId_actual"] == value]
        print(data.shape[0]/data2.shape[0])

    ana['run']= ana['reflist_run_id'].apply(lambda x:x.split('_')[1]).astype('int64')
    ong = ana.drop('reflist_run_id', axis=1)  # Supprimer la colonne 'reflist_run_id'
    ong_filtered = ong[ong["pred_ana_bool"] == True]  # Filtrer les lignes où "pred_ana_bool" est True
    grp1 = ong_filtered.groupby(['run', 'refListId_actual'])  # Groupement par 'run' et 'refListId_actual'
    grp1.sum()  # Calcul de la somme pour chaque groupe
    #grp1.sum().to_excel("xcel.xlsx",sheet_name="f")
    ong = ana.drop('reflist_run_id', axis=1)  # Supprimer la colonne 'reflist_run_id'
    ong_filtered = ong  # Filtrer les lignes où "pred_ana_bool" est True
    grp = ong_filtered.groupby(['run', 'refListId_actual'])['pred_ana_bool'].value_counts()\
    .unstack('pred_ana_bool',fill_value=0).reset_index(drop=False)
    # Renommer les colonnes
    grp = grp[['run', 'refListId_actual', True, False]]
    import matplotlib.pyplot as plt
    for index, row in grp.iterrows():
        # Récupération des valeurs de 'run', 'refListId_actual', True et False pour cette ligne
        run_value = row['run']
        reflist_value = row['refListId_actual']
        true_value = row[True]
        false_value = row[False]
        
        # Création d'un histogramme
        plt.figure()
        plt.bar(['Bien classés', 'Mal classés'], [true_value, false_value])
        plt.title(f'Tour {run_value}, {reflist_value+1}eme Boite')
        plt.xlabel('Valeur de pred_ana_bool')
        plt.ylabel('Nombre de Tags')
        plt.show()


