package com.sagita;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class CSVDataImporter {

	public void read() throws ClassNotFoundException, SQLException {

//		String path = getClass().getClassLoader().getResource("data.csv");
		Connection con = DBConector.getCsvConnectionInstance("E:\\repos\\Database-Apps\\oracleImport\\src\\main\\resources\\");

		Statement stmt = con.createStatement();
		ResultSet rs = stmt.executeQuery("SELECT hei,id FROM data");
		while (rs.next()) {
			System.out.println(rs.getString("hei"));
			System.out.println(rs.getString("id"));
		}

	}

}
