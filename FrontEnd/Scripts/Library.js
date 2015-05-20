var submitted = false;

function CheckSubmission()
{
	if(!submitted)
	{
		submitted = true;
		return true;
	}
	else
	{
		return false;
	}
}

function CheckTextBoxAgainstMaxLength(txtBox, size)
{
	if (txtBox.value.length >= size)
	{
		alert("Entry can't be longer than " + size + " characters");
		return false;
	}
	else
		return true;
}