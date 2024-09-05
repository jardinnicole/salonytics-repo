import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sqlalchemy import create_engine

# Step 1: Connect to SQL Server using SQLAlchemy
server = 'DESKTOP-RKU38ET'  
database = 'salonyticsDB'  
driver = 'SQL Server'  

# Connection string
conn_str = f'mssql+pyodbc://@{server}/{database}?driver={driver}'
engine = create_engine(conn_str)

# Step 2: Query the database and load data into a DataFrame
query = '''
    SELECT Stylist, AVG(CAST(Rating AS FLOAT)) AS Average_Rating
    FROM Appointments
    GROUP BY Stylist;
'''

df_stylist_ratings = pd.read_sql(query, engine)

# Step 3: Rank the stylists based on average ratings
df_stylist_ratings = df_stylist_ratings.sort_values(by='Average_Rating', ascending=False).reset_index(drop=True)

# Step 4: Create HTML report
html_report = f'''
<!DOCTYPE html>
<html>
<head>
    <title>Top Rated Stylist Report</title>
    <style>
        table {{
            border-collapse: collapse;
            width: 50%;
            margin-left: auto;
            margin-right: auto;
        }}
        th, td {{
            border: 1px solid black;
            padding: 8px;
            text-align: left;
        }}
        th {{
            background-color: #f2f2f2;
        }}
        h1, h2 {{
            text-align: center;
        }}
    </style>
</head>
<body>
    <h1>Top Rated Stylist Report</h1>
    <h2>Top Rated Stylist Information</h2>
    <table>
        <tr><th>Stylist</th><td>{df_stylist_ratings.iloc[0]['Stylist']}</td></tr>
        <tr><th>Average Rating</th><td>{df_stylist_ratings.iloc[0]['Average_Rating']:.2f}</td></tr>
    </table>
    <h2>Stylist Rankings by Average Rating</h2>
    <table>
        <tr><th>Rank</th><th>Stylist</th><th>Average Rating</th></tr>
'''

# Append stylist rankings to the report
for index, row in df_stylist_ratings.iterrows():
    html_report += f"        <tr><td>{index + 1}</td><td>{row['Stylist']}</td><td>{row['Average_Rating']:.2f}</td></tr>\n"

# Close the HTML document
html_report += '''
    </table>
</body>
</html>
'''

# Save the HTML report to a file
with open('top_rated_stylist_report.html', 'w') as file:
    file.write(html_report)
        
# Open the HTML report in the default web browser
import webbrowser
webbrowser.open('top_rated_stylist_report.html')

print("\nData analysis and reporting completed successfully.")
