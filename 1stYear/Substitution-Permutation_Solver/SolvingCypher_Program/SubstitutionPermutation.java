import java.util.Scanner;

public class SubstitutionPermutation {
		private static final int[][] cypherKeys = 
		{
			{1, 1, 0, 0, 0, 1, 0},
			{1, 1, 1, 1, 0, 1, 1},
			{0, 0, 0, 0, 0, 1, 1},
			{1, 1, 0, 0, 0, 1, 1}
		};
    private Scanner input;
		private String text;
		private int[] textArray;

	public SubstitutionPermutation() {
		input = new Scanner(System.in);
		text = "";
		textArray = new int[0];
	} //SubstitutionPermutation()

	public static void main(String[] args) {
		SubstitutionPermutation subPer = new SubstitutionPermutation();

		System.out.print("\nEnter the binary you want to encrypt: ");
		subPer.text = subPer.input.next();
		
		subPer.textArray = subPer.convertToIntArray(subPer.text);
		System.out.println("\nEncryted: " + subPer.encryptText(subPer.textArray));
		
		subPer.input.close();
  } //main(String[])

	private int[] convertToIntArray(String textPar) {
		int[] temp = new int[textPar.length()];

		if (temp.length == 4) {
			for(int i = 0; i < temp.length; i++) {
				if (textPar.charAt(i) == '0' || textPar.charAt(i) == '1')
					temp[i] = Integer.parseInt((textPar.charAt(i)) + "");
				else {
					System.err.println("Must Only Contain 1 or 0.");
					System.exit(0);
				}
			}
		} else {
			System.err.println("Too long.\n Must Be 4 Binary Characters.");
			System.exit(0);
		}

		return temp;
	} //covertToIntArray(String)

	private String encryptText(int[] intTextArray) {
		String output = "";
		intTextArray = switchMiddleNums(intTextArray);

		for(int x = 0; x < cypherKeys.length; x++) {
			int temp = intTextArray[x];
			for(int y = 0; y < cypherKeys[x].length; y++) {
				System.out.println("remainder: " + temp);
				System.out.println("key: " + cypherKeys[x][y]);
				System.out.println("answer: " + (temp + cypherKeys[x][y]) % 2);
				temp = (temp + cypherKeys[x][y]) % 2;
				System.out.println("\n");
			}
			System.out.println("Next Values:\n");
			output += temp;
		}
		
		return output;
	} //encryptText(int[])

		private int[] switchMiddleNums(int[] arrayInt) {
			int[] output = arrayInt;
				
			int temp = output[1];
			output[1] = output[2];
			output[2] = temp;

			return output;
		} //switchMiddleNums(int[])
}// SubstitutionPermutation class







	


