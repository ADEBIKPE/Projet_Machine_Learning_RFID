
def SVM_method(regularisation, CoefNoyau, n_plis, Noyau,pathfile):
    # Charger les données
    from preprocessing import preprocessing
    from sklearn.model_selection import cross_val_score
    from sklearn.svm import SVC
    from sklearn.metrics import confusion_matrix
    from sklearn.preprocessing import LabelEncoder
    from selectfeatures import Xcols_func
    import time
    start_time = time.time()
    X=Xcols_func('rssi & rc only',(preprocessing(pathfile)).columns)
    ds=preprocessing(pathfile)
    x = ds.loc[:, X]
    y = ds['in_or_out']
    # Créer un encodeur LabelEncoder
    label_encoder = LabelEncoder()

    # Encoder la variable cible
    y_encoded = label_encoder.fit_transform(y)
    
    # Créer le modèle
    svm_model = SVC(C=regularisation,gamma=CoefNoyau,kernel=Noyau) 
    
    # Effectuer une validation croisée
    scores = cross_val_score(svm_model, x, y_encoded, cv=n_plis)  # cv=n_plis indique une validation croisée avec n_plis plis
    
    # Afficher les scores de validation croisée
    print("Scores de validation croisée:", scores)
    print("Score moyen de validation croisée:", scores.mean())
    score=scores.mean()
    print("Score", score)
    # Entraîner le modèle sur toutes les données
    svm_model.fit(x, y_encoded)
    

    # Prédiction avec la validation croisée
    from sklearn.model_selection import cross_val_predict
    y_pred = cross_val_predict(svm_model, x, y_encoded, cv=n_plis)
    faux_inside = [i for i in range(len(y)) if y_pred[i] == 1 and y_encoded[i] == 0]
    faux_outside = [i for i in range(len(y)) if y_pred[i] == 0 and y_encoded[i] == 1]
    import pandas as pd

    # Nom de la colonne que vous souhaitez extraire
    column_names = ['Epc','reflist_run_id','refListId_actual']  # Remplacez 'nom_de_la_colonne' par le nom de la colonne que vous souhaitez extraire
    for colonne in column_names :
    
            # Obtenir l'indice de la colonne à partir de son nom
        column_index = pd.Index(ds.columns).get_loc(colonne)
        if colonne=='Epc':
            # Extraire les colonnes spécifiques de x_train associées à chaque type

            false_inside_column = ds.iloc[faux_inside, column_index]
            false_outside_column =ds.iloc[faux_outside, column_index]
        elif colonne == 'reflist_run_id':
            # Obtenir l'indice de la colonne à partir de son nom
            

            # Extraire les colonnes spécifiques de x_train associées à chaque type

            false_inside_column1 =[elem.split('_')[0] for elem in ds.iloc[faux_inside,  column_index]]
            false_outside_column1 =[elem.split('_')[0] for elem in ds.iloc[faux_outside,  column_index]]

        else:
            false_inside_column2 = ds.iloc[faux_inside, column_index]
            false_outside_column2=ds.iloc[faux_outside, column_index]
    details_classement = pd.DataFrame({'Tags':  false_inside_column, 'Classé dans la boîte':  false_inside_column1, 'Devrait être classé dans la boite':  false_inside_column2})


    # Calcul de la matrice de confusion
    conf_matrix = confusion_matrix(y_encoded, y_pred)

    #print("Confusion Matrix:")
    #print(conf_matrix)
    end_time = time.time()

    # Calcul du temps d'exécution
    execution_time = end_time - start_time

    return score, conf_matrix, execution_time, details_classement
    

