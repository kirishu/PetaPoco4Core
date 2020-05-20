<#
	run for "Execute as Script"
#>
# ---------------------------
# Version value
$major = 4
$miner = 7
$revision = 1.5
# ---------------------------

# target files and line number
$files = @{
	# ���ꂼ��̃t�@�C���ŏ���������s�ԍ���ݒ�
	"DatabaseExtension.cs" = [int]4
	"PetaPoco.cs" = [int]12
	"PetaPocoAttributes.cs" = [int]12
}
# ---------------------------

# �J�����g�p�X�擾
$currentPath = (Split-Path -Parent $MyInvocation.MyCommand.Path)
Write-Output ($currentPath)

# BOM�t���t�@�C���������݂̂��߁A�G���R�[�h��UTF8Encoding(true)���w��
$enc = New-Object System.Text.UTF8Encoding(-1)

foreach($file in $files.Keys){
	# read file
	[string] $filenm = Join-Path $currentPath $file
	Write-Output ($filenm)
	$lines = [System.IO.File]::ReadAllLines($filenm, $enc)
	
	[int] $rowix = $files[$file] - 1
	Write-Output ($lines[$rowix])

	# Write file
	$newver = " * v" + $major + "." + $miner +  "." + $revision
	$lines[$rowix] = $newver
	[System.IO.File]::WriteAllLines($filenm, $lines, $enc)
}

