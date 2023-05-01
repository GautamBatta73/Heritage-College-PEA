@Echo off
Pushd "%~dp0"
javac SubstitutionPermutation.java
@Echo on
java SubstitutionPermutation
@Echo off
popd
pause
