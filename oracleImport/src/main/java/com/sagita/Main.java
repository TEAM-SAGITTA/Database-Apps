package com.sagita;

import java.util.HashMap;
import java.util.Map;

public class Main {

	public static void main(String[] args) {
		try {

			OracleSchemaCreator osc = new OracleSchemaCreator();
			

			 String[] tables = { "MEASURES", "VENDORS", "PRODUCTS" };
			 Map<String, String> fkTableMap = new HashMap<>();
			 fkTableMap.put("PRODUCT_MEASURE", "PRODUCTS");
			 fkTableMap.put("PRODUCT_VENDOR", "PRODUCTS");
			 String[] sequences = { "ID_INCREMENTOR_SEQ" };
			 osc.dropIfExist(tables, fkTableMap, sequences);
			
			 
			 osc.create();
			
			CSVDataImporter csv=new CSVDataImporter();
			//csv.read();
		
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
