package com.sagita;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class DBConector {

	private static Connection oracleConnection;
	private static Connection csvConnection;

	public static synchronized Connection getOracleConectionInstance()
			throws SQLException, ClassNotFoundException {
		if (oracleConnection == null || oracleConnection.isClosed()) {
			Class.forName("oracle.jdbc.driver.OracleDriver");
			oracleConnection = DriverManager.getConnection(
					"jdbc:oracle:thin:@localhost:1521/xe", "softuni", "zkalev");
		}
		return oracleConnection;
	}

	public static synchronized Connection getCsvConnectionInstance(String dir)
			throws ClassNotFoundException, SQLException {
		java.util.Properties props = new java.util.Properties();
		 
		props.put("separator", ",");              // separator is a bar
		props.put("suppressHeaders", "false");    // column headers are on the first line
		props.put("fileExtension", ".csv");       // default file extension is .csv
		props.put("charset", "ISO-8859-2");       // file encoding is "ISO-8859-2"
		props.put("commentLine", "--");           // string denoting comment line is "--"
		 
		Class.forName("org.relique.jdbc.csv.CsvDriver");
		Connection conn = DriverManager.getConnection("jdbc:relique:csv:"
				+ dir,props);

		return conn;
	}
}
