import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sqlalchemy import create_engine
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestRegressor
from sklearn.metrics import mean_squared_error
from jinja2 import Template
import webbrowser  # Import the webbrowser module

# Step 1: Connect to SQL Server using SQLAlchemy
server = 'DESKTOP-RKU38ET'  
database = 'salonyticsDB'  
driver = 'SQL Server'  

# connection string
conn_str = f'mssql+pyodbc://@{server}/{database}?driver={driver}'
engine = create_engine(conn_str)

# Step 2: Query the database and load data into a DataFrame
def query_to_df(query, engine):
    df = pd.read_sql(query, engine)
    return df

# query to retrieve data
query = '''
    SELECT Branch, Stylist, Date, Customer_age, Customer_Gender, Service
    FROM Appointments
'''

df = query_to_df(query, engine)

# Step 4: Analyze and manipulate data
# Clean up column names and data types
df.columns = ['branch', 'stylist', 'date', 'customer_age', 'gender', 'service']
df['date'] = pd.to_datetime(df['date'], format='%Y-%m-%d')
df['branch'] = df['branch'].astype('category')
df['gender'] = df['gender'].astype('category')
df['service'] = df['service'].astype('category')
df = df.sort_values('date').reset_index(drop=True)

# Step 5: Exploratory Data Analysis (EDA)
# Plot the count of appointments by date
plt.figure(figsize=(12, 6))
sns.countplot(x=df['date'].dt.date)
plt.title('Count of Appointments by Date')
plt.xlabel('Date')
plt.ylabel('Count')
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig('count_appointments_by_date.png')  # Save the plot as an image
plt.close()  # Close the plot to free up memory

# Plot the count of appointments by branch
plt.figure(figsize=(10, 5))
sns.countplot(x='branch', data=df)
plt.title('Count of Appointments by Branch')
plt.xlabel('Branch')
plt.ylabel('Count')
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig('count_appointments_by_branch.png')  # Save the plot as an image
plt.close()  # Close the plot to free up memory

# Plot the count of appointments by gender
plt.figure(figsize=(6, 4))
sns.countplot(x='gender', data=df)
plt.title('Count of Appointments by Gender')
plt.xlabel('Gender')
plt.ylabel('Count')
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig('count_appointments_by_gender.png')  # Save the plot as an image
plt.close()  # Close the plot to free up memory

# Step 6: Predictive Modeling
# Encode categorical variables
df['gender_code'] = df['gender'].cat.codes
df['service_code'] = df['service'].cat.codes
df['branch_code'] = df['branch'].cat.codes

# Extract month and year from 'date' column
df['Month'] = df['date'].dt.month
df['Year'] = df['date'].dt.year

# Define features and target variable for regression
monthly_services = df.groupby(['Year', 'Month']).agg({
    'date': 'count',
}).reset_index().rename(columns={'date': 'Number_of_Services'})

# Include profitability data
profitability_data = pd.DataFrame({
    'Month': [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
    'Relative_Profitability': [0.556, 0.444, 0.722, 0.889, 0.945, 0.833, 0.889, 0.833, 0.778, 0.722, 0.667, 1.000]
})

X_regress = monthly_services[['Month', 'Year']]
y_regress = monthly_services['Number_of_Services']

X_regress = pd.merge(X_regress, profitability_data, on='Month')

# Split the data into training and testing sets for regression
X_train_regress, X_test_regress, y_train_regress, y_test_regress = train_test_split(X_regress, y_regress, test_size=0.2, random_state=42)

# Initialize a RandomForestRegressor
regressor = RandomForestRegressor(random_state=42)

# Train the model
regressor.fit(X_train_regress, y_train_regress)

# Predict for regression
y_pred_regress = regressor.predict(X_test_regress)

# Evaluate the regression model
regression_mean_squared_error = mean_squared_error(y_test_regress, y_pred_regress)

# Feature importances for regression
feature_importances_regress = pd.DataFrame(regressor.feature_importances_,
                                           index=X_train_regress.columns,
                                           columns=['importance']).sort_values('importance', ascending=False)

# Determine the latest year and month in the data
latest_year = df['Year'].max()
latest_month = df[df['Year'] == latest_year]['Month'].max()

# Calculate future months for which predictions are needed
future_months = []
for year in range(latest_year, latest_year + 2):  # Predict for next 2 years
    for month in range(1, 13):
        if year == latest_year and month <= latest_month:
            continue
        future_months.append({'Month': month, 'Year': year})

# Convert future_months to DataFrame
future_months_df = pd.DataFrame(future_months)

# Merge future_months_df with profitability_data
future_months_df = pd.merge(future_months_df, profitability_data, on='Month')

# Predictions for future months
future_predictions = regressor.predict(future_months_df)

# Adjust predictions based on relative profitability
future_months_df['Predicted_Services'] = future_predictions * future_months_df['Relative_Profitability']
future_months_df['Predicted_Services'] = future_months_df['Predicted_Services'].round(0).astype(int)

# Step 8: Plotting the average number of services availed per day of the month
# Count services per day
daily_services = df.groupby(['Year', 'Month', 'date']).size().reset_index(name='count')
daily_services['day'] = daily_services['date'].dt.day

# Calculate average services per day of the month
average_services_per_day = daily_services.groupby('day')['count'].mean().reset_index()

# Plotting the average number of services per day of the month
plt.figure(figsize=(12, 6))
sns.barplot(x='day', y='count', data=average_services_per_day, palette='viridis')
plt.title('Average Number of Services Availed per Day of the Month')
plt.xlabel('Day of the Month')
plt.ylabel('Average Number of Services')
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig('average_services_per_day.png')  # Save the plot as an image
plt.close()  # Close the plot to free up memory

# Step 9: Create HTML Report with Data
# Prepare data for HTML report
report_data = {
    'regression_mean_squared_error': regression_mean_squared_error,
    'regression_feature_importances': feature_importances_regress.to_html(),
    'monthly_services': monthly_services.to_html(),
    'future_predictions': future_months_df.to_html()
}

# HTML template for the report
template = Template('''
<!DOCTYPE html>
<html>
<head>
    <title>Data Analysis Report</title>
    <style>
        table {
            border-collapse: collapse;
            width: 100%;
        }
        th, td {
            border: 1px solid black;
            padding: 8px;
            text-align: left;
        }
        th {
            background-color: #f2f2f2;
        }
        img {
            max-width: 100%;
            height: auto;
            display: block;
            margin-left: auto;
            margin-right: auto;
            margin-top: 10px;
            margin-bottom: 10px;
        }
        h2 {
            color: #4CAF50;
        }
        h3 {
            color: #2196F3;
        }
        p {
            color: #212121;
        }
        pre {
            white-space: pre-wrap;
        }
    </style>
</head>
<body>
    <h1>Data Analysis Report</h1>
    
    <h2>Summary</h2>
    <p>This report presents the analysis of appointments data and predictions for future services.</p>
    
    <h2>Plots</h2>
    <h3>Count of Appointments by Date</h3>
    <img src="count_appointments_by_date.png" alt="Count of Appointments by Date">
    
    <h3>Count of Appointments by Branch</h3>
    <img src="count_appointments_by_branch.png" alt="Count of Appointments by Branch">
    
    <h3>Count of Appointments by Gender</h3>
    <img src="count_appointments_by_gender.png" alt="Count of Appointments by Gender">
    
       <h3>Average Number of Services Availed per Day of the Month</h3>
    <img src="average_services_per_day.png" alt="Average Number of Services Availed per Day of the Month">
    
    <h2>Regression Results</h2>
    <p>Mean Squared Error (Regression): {{ regression_mean_squared_error }}</p>
    <h3>Feature Importances (Regression)</h3>
    {{ regression_feature_importances | safe }}
    
    <h2>Monthly Services Data</h2>
    {{ monthly_services | safe }}

    <h2>Predictions for Future Months</h2>
    {{ future_predictions | safe }}
    
</body>
</html>
''')

# Render the HTML template with the data
html_output = template.render(report_data)

# Save the HTML report to a file
report_file_path = 'data_analysis_report.html'
with open(report_file_path, 'w') as f:
    f.write(html_output)

# Open the HTML report file in the default web browser
webbrowser.open(report_file_path)



print('HTML report generated successfully and opened in the default web browser.')
